# Android Folder Import/Export Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Let the user choose a visible Android folder for every export and import, write one file per selected category, and import the newest matching files from that folder.

**Architecture:** Keep export data preparation in `ExportService`, but replace ordinary path-based I/O with stream-based serialization and an `IExportFolderService` abstraction. The Android implementation uses Storage Access Framework (`ACTION_OPEN_DOCUMENT_TREE`, `DocumentsContract`, and `ContentResolver`) so no broad external-storage permission is required.

**Tech Stack:** .NET 10, .NET MAUI 10, Android API 36 bindings, Android Storage Access Framework, MSTest, XML serialization.

---

## File map

- Modify `Contracts/Enums/ExportCategories.cs`: make the flags independent.
- Create `Infrastructure/Infrastructure/Services/ExportFolderFile.cs`: platform-neutral document metadata.
- Create `Infrastructure/Infrastructure/Services/IExportFolderService.cs`: folder picker and stream access boundary.
- Create `Infrastructure/Infrastructure/Services/ExportOperationResult.cs`: exported filenames.
- Create `Infrastructure/Infrastructure/Services/ImportOperationResult.cs`: imported filenames and missing categories.
- Modify `Infrastructure/Infrastructure/Services/IFileService.cs`: serialize and deserialize streams instead of paths.
- Modify `Infrastructure/Infrastructure/Services/IExportService.cs`: return operation summaries and remove explicit import paths.
- Create `Services/ExportFileNaming.cs`: filename creation and newest-file selection.
- Modify `Services/FileService.cs`: stream-based XML serialization.
- Modify `Services/ExportService.cs`: use folder streams and report partial imports.
- Create `Tests/Tests/Services/ExportFileNamingTests.cs`: flag and filename selection tests.
- Create `Tests/Tests/Services/FileServiceTests.cs`: stream serialization tests.
- Create `Tests/Tests/Services/ExportServiceFolderTests.cs`: export overwrite, newest import, partial import, and malformed XML tests.
- Create `TinkoffInvestStatistic/TinkoffInvestStatistic/Platforms/Android/Services/AndroidExportFolderService.cs`: Storage Access Framework implementation.
- Modify `TinkoffInvestStatistic/TinkoffInvestStatistic/Platforms/Android/MainActivity.cs`: route folder-picker results.
- Modify `TinkoffInvestStatistic/TinkoffInvestStatistic/App.xaml.cs`: explicitly register the Android folder service.
- Modify `TinkoffInvestStatistic/TinkoffInvestStatistic/ViewModels/ExportViewModel.cs`: choose a folder per operation and show summaries.
- Modify `TinkoffInvestStatistic/TinkoffInvestStatistic/Views/ExportPage.xaml`: simplify the screen to category checkboxes and two actions.
- Delete `TinkoffInvestStatistic/TinkoffInvestStatistic/Service/IFileSystemService.cs`: obsolete path API.
- Delete `TinkoffInvestStatistic/TinkoffInvestStatistic/Platforms/Android/Services/FileSystemService.cs`: obsolete direct-storage implementation.
- Modify `TinkoffInvestStatistic/TinkoffInvestStatistic/Platforms/Android/AndroidManifest.xml`: remove `WRITE_EXTERNAL_STORAGE` only; preserve the existing network-security changes.
- Create `Tests/Tests/Application/AndroidStorageConfigurationTests.cs`: guard against reintroducing broad storage permission.

### Task 1: Define storage contracts and deterministic file naming

**Files:**
- Modify: `Contracts/Enums/ExportCategories.cs`
- Create: `Infrastructure/Infrastructure/Services/ExportFolderFile.cs`
- Create: `Infrastructure/Infrastructure/Services/IExportFolderService.cs`
- Create: `Infrastructure/Infrastructure/Services/ExportOperationResult.cs`
- Create: `Infrastructure/Infrastructure/Services/ImportOperationResult.cs`
- Create: `Services/ExportFileNaming.cs`
- Test: `Tests/Tests/Services/ExportFileNamingTests.cs`

- [ ] **Step 1: Write failing flag and filename tests**

Create `Tests/Tests/Services/ExportFileNamingTests.cs`:

```csharp
using Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System;
using TinkoffInvestStatistic.Contracts.Enums;

namespace Tests.Services;

[TestClass]
public class ExportFileNamingTests
{
    [TestMethod]
    public void ExportCategories_AreIndependentFlags()
    {
        Assert.AreEqual(0, (int)(ExportCategories.Settings & ExportCategories.Data));
        Assert.AreEqual(0, (int)(ExportCategories.Settings & ExportCategories.Transfers));
        Assert.AreEqual(0, (int)(ExportCategories.Data & ExportCategories.Transfers));
    }

    [TestMethod]
    [DataRow(ExportCategories.Settings, "exported_Settings_11.08.2026.txt")]
    [DataRow(ExportCategories.Data, "exported_Data_11.08.2026.txt")]
    [DataRow(ExportCategories.Transfers, "exported_Transfers_11.08.2026.txt")]
    public void Create_CreatesExpectedName(ExportCategories category, string expected)
    {
        Assert.AreEqual(expected, ExportFileNaming.Create(category, new DateTime(2026, 8, 11)));
    }

    [TestMethod]
    public void FindLatest_IgnoresInvalidNamesAndUsesDateFromName()
    {
        var files = new[]
        {
            new ExportFolderFile("invalid", "exported_Settings_latest.txt", DateTimeOffset.MaxValue),
            new ExportFolderFile("old", "exported_Settings_10.08.2026.txt", DateTimeOffset.MaxValue),
            new ExportFolderFile("new", "exported_Settings_11.08.2026.txt", DateTimeOffset.MinValue),
            new ExportFolderFile("data", "exported_Data_12.08.2026.txt", DateTimeOffset.MaxValue),
        };

        Assert.AreEqual("new", ExportFileNaming.FindLatest(ExportCategories.Settings, files)?.Id);
    }

    [TestMethod]
    public void FindLatest_SameDateUsesLastModifiedThenStableId()
    {
        var timestamp = new DateTimeOffset(2026, 8, 11, 12, 0, 0, TimeSpan.Zero);
        var files = new[]
        {
            new ExportFolderFile("z", "exported_Settings_11.08.2026.txt", timestamp),
            new ExportFolderFile("a", "exported_Settings_11.08.2026.txt", timestamp),
            new ExportFolderFile("middle", "exported_Settings_11.08.2026.txt", timestamp.AddMinutes(1)),
        };

        Assert.AreEqual("middle", ExportFileNaming.FindLatest(ExportCategories.Settings, files)?.Id);
        Assert.AreEqual(
            "a",
            ExportFileNaming.FindLatest(ExportCategories.Settings, files[..2])?.Id);
    }
}
```

- [ ] **Step 2: Run the tests and verify that they fail**

Run:

```powershell
dotnet test Tests\Tests\Tests.csproj --filter FullyQualifiedName~ExportFileNamingTests
```

Expected: compilation fails because `ExportFileNaming` and `ExportFolderFile` do not exist; the independent-flags assertion would also fail because `Transfers` is currently `3`.

- [ ] **Step 3: Correct the flags and add the storage contracts**

In `Contracts/Enums/ExportCategories.cs`, change only the `Transfers` value:

```csharp
Transfers = 4,
```

Create `Infrastructure/Infrastructure/Services/ExportFolderFile.cs`:

```csharp
using System;

namespace Infrastructure.Services;

public sealed record ExportFolderFile(
    string Id,
    string Name,
    DateTimeOffset? LastModified);
```

Create `Infrastructure/Infrastructure/Services/IExportFolderService.cs`:

```csharp
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public interface IExportFolderService
{
    Task<string?> PickFolderAsync(CancellationToken cancellation);

    Task<IReadOnlyList<ExportFolderFile>> GetFilesAsync(
        string folderId,
        CancellationToken cancellation);

    Task<Stream> OpenReadAsync(string fileId, CancellationToken cancellation);

    Task<Stream> OpenWriteAsync(
        string folderId,
        string fileName,
        CancellationToken cancellation);
}
```

Create `Infrastructure/Infrastructure/Services/ExportOperationResult.cs`:

```csharp
using System.Collections.Generic;

namespace Infrastructure.Services;

public sealed record ExportOperationResult(IReadOnlyList<string> SavedFiles);
```

Create `Infrastructure/Infrastructure/Services/ImportOperationResult.cs`:

```csharp
using System.Collections.Generic;
using TinkoffInvestStatistic.Contracts.Enums;

namespace Infrastructure.Services;

public sealed record ImportOperationResult(
    IReadOnlyList<string> ImportedFiles,
    IReadOnlyList<ExportCategories> MissingCategories);
```

- [ ] **Step 4: Implement filename creation and selection**

Create `Services/ExportFileNaming.cs`:

```csharp
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TinkoffInvestStatistic.Contracts.Enums;

namespace Services;

public static class ExportFileNaming
{
    private const string DateFormat = "dd.MM.yyyy";
    private const string Suffix = ".txt";

    public static string Create(ExportCategories category, DateTime utcNow)
    {
        EnsureSingleCategory(category);
        return $"exported_{category}_{utcNow.ToString(DateFormat, CultureInfo.InvariantCulture)}{Suffix}";
    }

    public static ExportFolderFile? FindLatest(
        ExportCategories category,
        IEnumerable<ExportFolderFile> files)
    {
        EnsureSingleCategory(category);
        var prefix = $"exported_{category}_";
        var candidates = new List<(ExportFolderFile File, DateTime Date)>();

        foreach (var file in files)
        {
            if (!file.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ||
                !file.Name.EndsWith(Suffix, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var dateText = file.Name.Substring(
                prefix.Length,
                file.Name.Length - prefix.Length - Suffix.Length);
            if (DateTime.TryParseExact(
                    dateText,
                    DateFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date))
            {
                candidates.Add((file, date));
            }
        }

        return candidates
            .OrderByDescending(candidate => candidate.Date)
            .ThenByDescending(candidate => candidate.File.LastModified ?? DateTimeOffset.MinValue)
            .ThenBy(candidate => candidate.File.Id, StringComparer.Ordinal)
            .Select(candidate => candidate.File)
            .FirstOrDefault();
    }

    private static void EnsureSingleCategory(ExportCategories category)
    {
        if (category is not ExportCategories.Settings and
            not ExportCategories.Data and
            not ExportCategories.Transfers)
        {
            throw new ArgumentOutOfRangeException(nameof(category), category, "Expected one export category.");
        }
    }
}
```

- [ ] **Step 5: Run the focused tests**

Run:

```powershell
dotnet test Tests\Tests\Tests.csproj --filter FullyQualifiedName~ExportFileNamingTests
```

Expected: 5 tests pass.

- [ ] **Step 6: Commit the contracts and filename logic**

```powershell
git add Contracts/Enums/ExportCategories.cs Infrastructure/Infrastructure/Services/ExportFolderFile.cs Infrastructure/Infrastructure/Services/IExportFolderService.cs Infrastructure/Infrastructure/Services/ExportOperationResult.cs Infrastructure/Infrastructure/Services/ImportOperationResult.cs Services/ExportFileNaming.cs Tests/Tests/Services/ExportFileNamingTests.cs
git commit -m "feat: define export folder storage contracts"
```

### Task 2: Move XML serialization from paths to streams

**Files:**
- Modify: `Infrastructure/Infrastructure/Services/IFileService.cs`
- Modify: `Services/FileService.cs`
- Test: `Tests/Tests/Services/FileServiceTests.cs`

- [ ] **Step 1: Write the failing stream round-trip test**

Create `Tests/Tests/Services/FileServiceTests.cs`:

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvestStatistic.Contracts.Export;

namespace Tests.Services;

[TestClass]
public class FileServiceTests
{
    [TestMethod]
    public async Task SaveAndLoadFileAsync_RoundTripsXmlWithoutClosingCallerStream()
    {
        var service = new FileService();
        using var stream = new MemoryStream();
        var source = new[] { new OptionExportData(OptionType.IsHideMoney, "true") };

        await service.SaveFileAsync(source, stream, CancellationToken.None);

        Assert.IsTrue(stream.CanRead);
        stream.Position = 0;
        var restored = await service.LoadFileAsync<OptionExportData[]>(stream, CancellationToken.None);

        Assert.AreEqual(1, restored.Length);
        Assert.AreEqual(OptionType.IsHideMoney, restored[0].Type);
        Assert.AreEqual("true", restored[0].Value);
        Assert.IsTrue(stream.CanRead);
    }
}
```

- [ ] **Step 2: Run the test and verify that it fails**

Run:

```powershell
dotnet test Tests\Tests\Tests.csproj --filter FullyQualifiedName~FileServiceTests
```

Expected: compilation fails because `IFileService` and `FileService` still accept string paths.

- [ ] **Step 3: Replace path methods with stream methods**

Replace `Infrastructure/Infrastructure/Services/IFileService.cs` with:

```csharp
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public interface IFileService
{
    Task SaveFileAsync(object data, Stream output, CancellationToken cancellation);

    Task<T> LoadFileAsync<T>(Stream input, CancellationToken cancellation);
}
```

Replace `Services/FileService.cs` with:

```csharp
using Infrastructure.Services;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services;

public class FileService : IFileService
{
    public async Task SaveFileAsync(object data, Stream output, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        var serializer = new XmlSerializer(data.GetType());
        using var writer = new StreamWriter(
            output,
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: true),
            bufferSize: 1024,
            leaveOpen: true);

        serializer.Serialize(writer, data);
        await writer.FlushAsync(cancellation);
    }

    public Task<T> LoadFileAsync<T>(Stream input, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StreamReader(
            input,
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: true,
            bufferSize: 1024,
            leaveOpen: true);
        var data = serializer.Deserialize(reader);
        cancellation.ThrowIfCancellationRequested();

        if (data is not T typedData)
        {
            throw new ApplicationException("Не удалось прочитать экспортированный файл.");
        }

        return Task.FromResult(typedData);
    }
}
```

- [ ] **Step 4: Run the stream test**

Run:

```powershell
dotnet test Tests\Tests\Tests.csproj --filter FullyQualifiedName~FileServiceTests
```

Expected: 1 test passes.

- [ ] **Step 5: Commit stream serialization**

```powershell
git add Infrastructure/Infrastructure/Services/IFileService.cs Services/FileService.cs Tests/Tests/Services/FileServiceTests.cs
git commit -m "refactor: serialize export data through streams"
```

### Task 3: Refactor ExportService around the selected folder

**Files:**
- Modify: `Infrastructure/Infrastructure/Services/IExportService.cs`
- Modify: `Services/ExportService.cs`
- Test: `Tests/Tests/Services/ExportServiceFolderTests.cs`

- [ ] **Step 1: Write folder-flow tests with in-memory fakes**

Create `Tests/Tests/Services/ExportServiceFolderTests.cs`. The test class must cover these exact cases:

```csharp
[TestMethod]
public async Task ExportAsync_SameDayReplacesSameSettingsFile()
{
    var fixture = new ExportFixture();
    await fixture.Service.ExportAsync(ExportCategories.Settings, "folder", CancellationToken.None);
    fixture.Settings.Value = "false";
    var result = await fixture.Service.ExportAsync(ExportCategories.Settings, "folder", CancellationToken.None);

    CollectionAssert.AreEqual(
        new[] { "exported_Settings_11.08.2026.txt" },
        result.SavedFiles.ToArray());
    Assert.AreEqual(1, fixture.Folder.FileCount);
}

[TestMethod]
public async Task ImportAsync_UsesNewestSettingsAndReportsMissingData()
{
    var fixture = new ExportFixture();
    await fixture.Folder.PutSettingsAsync("exported_Settings_10.08.2026.txt", "old");
    await fixture.Folder.PutSettingsAsync("exported_Settings_11.08.2026.txt", "new");

    var result = await fixture.Service.ImportAsync(
        ExportCategories.Settings | ExportCategories.Data,
        "folder",
        CancellationToken.None);

    Assert.AreEqual("new", fixture.Settings.UpdatedValue);
    CollectionAssert.AreEqual(
        new[] { "exported_Settings_11.08.2026.txt" },
        result.ImportedFiles.ToArray());
    CollectionAssert.AreEqual(
        new[] { ExportCategories.Data },
        result.MissingCategories.ToArray());
}

[TestMethod]
public async Task ImportAsync_NoMatchingFilesDoesNotChangeSettings()
{
    var fixture = new ExportFixture();

    var result = await fixture.Service.ImportAsync(
        ExportCategories.Settings,
        "folder",
        CancellationToken.None);

    Assert.IsNull(fixture.Settings.UpdatedValue);
    Assert.AreEqual(0, result.ImportedFiles.Count);
    CollectionAssert.AreEqual(
        new[] { ExportCategories.Settings },
        result.MissingCategories.ToArray());
}

[TestMethod]
public async Task ImportAsync_InvalidXmlDoesNotChangeSettings()
{
    var fixture = new ExportFixture();
    fixture.Folder.PutRaw("exported_Settings_11.08.2026.txt", "not xml");

    await Assert.ThrowsExceptionAsync<ApplicationException>(() => fixture.Service.ImportAsync(
        ExportCategories.Settings,
        "folder",
        CancellationToken.None));

    Assert.IsNull(fixture.Settings.UpdatedValue);
}
```

In the same test file, implement `ExportFixture`, `InMemoryExportFolderService`, `FakeSettingService`, and `FixedDateTimeProvider` as private nested classes. `InMemoryExportFolderService` stores `MemoryStream` instances by filename, returns the filename as `ExportFolderFile.Id`, replaces the dictionary entry from `OpenWriteAsync`, and returns a new readable `MemoryStream` from `OpenReadAsync`. `PutSettingsAsync` must serialize an `OptionExportData[]` using the real `FileService`; `FakeSettingService` must implement all three `ISettingService` members and record `UpdatedValue`; `FixedDateTimeProvider.UtcNow` must return `new DateTime(2026, 8, 11)`.

- [ ] **Step 2: Run the tests and verify that they fail**

Run:

```powershell
dotnet test Tests\Tests\Tests.csproj --filter FullyQualifiedName~ExportServiceFolderTests
```

Expected: compilation fails because the current service returns `Task`, uses filesystem paths, and has no injectable folder service.

- [ ] **Step 3: Change the public export service contract**

Replace the two methods in `Infrastructure/Infrastructure/Services/IExportService.cs` with:

```csharp
Task<ExportOperationResult> ExportAsync(
    ExportCategories categories,
    string folderId,
    CancellationToken cancellation);

Task<ImportOperationResult> ImportAsync(
    ExportCategories categories,
    string folderId,
    CancellationToken cancellation);
```

Delete the `settingsPath`, `dataPath`, and `transfersPath` parameters and their XML comments.

- [ ] **Step 4: Inject folder storage and return an export summary**

At the top of `Services/ExportService.cs`, rename `_fileservice` to `_fileService`, add `_folderService`, and use these constructors:

```csharp
private readonly IFileService _fileService;
private readonly IExportFolderService _folderService;
private readonly IDateTimeProvider _dateTimeProvider;
private readonly ISettingService _settingService;

public ExportService()
    : this(
        RequireService<IFileService>(),
        RequireService<IExportFolderService>(),
        RequireService<IDateTimeProvider>(),
        RequireService<ISettingService>())
{
}

public ExportService(
    IFileService fileService,
    IExportFolderService folderService,
    IDateTimeProvider dateTimeProvider,
    ISettingService settingService)
{
    _fileService = fileService;
    _folderService = folderService;
    _dateTimeProvider = dateTimeProvider;
    _settingService = settingService;
}

private static T RequireService<T>() where T : class
{
    return DependencyService.Get<T>()
        ?? throw new InvalidOperationException($"Сервис {typeof(T).Name} не зарегистрирован.");
}
```

Replace `ExportAsync` and the three save methods with implementations that collect filenames. Use this complete shared writer:

```csharp
private async Task<string> SaveFileAsync(
    ExportCategories category,
    object data,
    string folderId,
    CancellationToken cancellation)
{
    var fileName = ExportFileNaming.Create(category, _dateTimeProvider.UtcNow);
    try
    {
        await using var output = await _folderService.OpenWriteAsync(
            folderId,
            fileName,
            cancellation);
        await _fileService.SaveFileAsync(data, output, cancellation);
        return fileName;
    }
    catch (OperationCanceledException)
    {
        throw;
    }
    catch (Exception exception)
    {
        throw new ApplicationException(
            $"Не удалось сохранить файл {fileName}: {exception.Message}",
            exception);
    }
}
```

`ExportAsync` must invoke settings, data, and transfers in that order only when the corresponding independent flag is set, add each returned name to a `List<string>`, and return `new ExportOperationResult(savedFiles)`. Keep the existing data-building methods unchanged.

- [ ] **Step 5: Replace path discovery with folder enumeration**

Implement `ImportAsync` as:

```csharp
public async Task<ImportOperationResult> ImportAsync(
    ExportCategories categories,
    string folderId,
    CancellationToken cancellation)
{
    var files = await _folderService.GetFilesAsync(folderId, cancellation);
    var importedFiles = new List<string>();
    var missingCategories = new List<ExportCategories>();

    await ImportSettingsAsync(categories, files, importedFiles, missingCategories, cancellation);
    await ImportDataAsync(categories, files, importedFiles, missingCategories, cancellation);
    await ImportTransfersAsync(categories, files, importedFiles, missingCategories, cancellation);

    return new ImportOperationResult(importedFiles, missingCategories);
}
```

For each import method:

1. Return immediately if its flag is absent.
2. Call `ExportFileNaming.FindLatest(category, files)`.
3. Add the category to `missingCategories` and return if no file is found.
4. Open the selected `file.Id` with `_folderService.OpenReadAsync`.
5. Deserialize it with `_fileService.LoadFileAsync<T>`.
6. Perform the existing update logic only after successful deserialization.
7. Add `file.Name` to `importedFiles` after the category is fully imported.
8. Wrap non-cancellation exceptions as `ApplicationException($"Не удалось импортировать файл {file.Name}: {exception.Message}", exception)`.

Delete `GetLatestFilePath` and remove `System.IO` if no longer used elsewhere in the file.

- [ ] **Step 6: Run focused and full service tests**

Run:

```powershell
dotnet test Tests\Tests\Tests.csproj --filter "FullyQualifiedName~ExportServiceFolderTests|FullyQualifiedName~ExportFileNamingTests|FullyQualifiedName~FileServiceTests"
dotnet test Tests\Tests\Tests.csproj
```

Expected: all new tests pass; the full suite finishes with zero failed tests.

- [ ] **Step 7: Commit the service refactor**

```powershell
git add Infrastructure/Infrastructure/Services/IExportService.cs Services/ExportService.cs Tests/Tests/Services/ExportServiceFolderTests.cs
git commit -m "feat: import and export through selected folders"
```

### Task 4: Implement Android Storage Access Framework folder access

**Files:**
- Create: `TinkoffInvestStatistic/TinkoffInvestStatistic/Platforms/Android/Services/AndroidExportFolderService.cs`
- Modify: `TinkoffInvestStatistic/TinkoffInvestStatistic/Platforms/Android/MainActivity.cs`
- Modify: `TinkoffInvestStatistic/TinkoffInvestStatistic/App.xaml.cs`
- Delete: `TinkoffInvestStatistic/TinkoffInvestStatistic/Service/IFileSystemService.cs`
- Delete: `TinkoffInvestStatistic/TinkoffInvestStatistic/Platforms/Android/Services/FileSystemService.cs`

- [ ] **Step 1: Add the Android folder service**

Create `AndroidExportFolderService` implementing `IExportFolderService` with these rules:

- `PickFolderAsync` starts an `Intent.ActionOpenDocumentTree` intent on `Platform.CurrentActivity` with `GrantReadUriPermission | GrantWriteUriPermission`.
- A single static `TaskCompletionSource<string?>` represents the outstanding picker request; a second concurrent request throws `InvalidOperationException`.
- Request code is a private constant, for example `4317`.
- `TryHandleActivityResult` returns `false` for other request codes, returns `null` for cancellation, and returns `data.Data.ToString()` for success.
- Cancellation completes the pending task as canceled and always clears the static pending field in `finally`.
- `GetFilesAsync` queries the child-document URI built with `DocumentsContract.BuildChildDocumentsUriUsingTree`, using `ColumnDocumentId`, `ColumnDisplayName`, and `ColumnLastModified`. Each returned `Id` is a full document URI built with `BuildDocumentUriUsingTree`.
- `OpenReadAsync` uses `ContentResolver.OpenInputStream` and throws a descriptive `IOException` if it returns null.
- `OpenWriteAsync` searches `GetFilesAsync` case-insensitively. For an existing file, open its URI with mode `wt`; otherwise create `text/plain` with `DocumentsContract.CreateDocument` under the tree document URI, then open it with mode `wt`.
- All public methods call `cancellation.ThrowIfCancellationRequested()` before synchronous Android calls.

Do not add `Xamarin.AndroidX.DocumentFile`; the .NET Android 36 reference pack already contains the required `DocumentsContract` and `ContentResolver` APIs.

- [ ] **Step 2: Route picker results from MainActivity**

Add this override to `Platforms/Android/MainActivity.cs` and import the Android service namespace:

```csharp
protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
{
    if (!AndroidExportFolderService.TryHandleActivityResult(requestCode, resultCode, data))
    {
        base.OnActivityResult(requestCode, resultCode, data);
    }
}
```

Mark `TryHandleActivityResult` as `internal static` in the service.

- [ ] **Step 3: Register the concrete service explicitly**

In `App.ConfigureUtility`, add:

```csharp
DependencyService.Register<IExportFolderService, AndroidExportFolderService>();
```

Add the needed `using TinkoffInvestStatistic.Droid.Services;`. Do not rely on `[assembly: Dependency]`, because that discovery failed for the previous `IFileSystemService` in the release APK.

- [ ] **Step 4: Remove obsolete direct-path services**

Delete:

```text
TinkoffInvestStatistic/TinkoffInvestStatistic/Service/IFileSystemService.cs
TinkoffInvestStatistic/TinkoffInvestStatistic/Platforms/Android/Services/FileSystemService.cs
```

Run `rg -n "IFileSystemService|GetExternalStorage|FileSystemService" TinkoffInvestStatistic` and expect no matches.

- [ ] **Step 5: Compile the Android project**

Run:

```powershell
dotnet build TinkoffInvestStatistic\TinkoffInvestStatistic\TinkoffInvestStatistic.csproj -c Debug --no-restore
```

Expected: build succeeds with zero errors. Existing package vulnerability warnings may remain and are outside this feature.

- [ ] **Step 6: Commit the Android storage implementation**

```powershell
git add TinkoffInvestStatistic/TinkoffInvestStatistic/Platforms/Android/Services/AndroidExportFolderService.cs TinkoffInvestStatistic/TinkoffInvestStatistic/Platforms/Android/MainActivity.cs TinkoffInvestStatistic/TinkoffInvestStatistic/App.xaml.cs TinkoffInvestStatistic/TinkoffInvestStatistic/Service/IFileSystemService.cs TinkoffInvestStatistic/TinkoffInvestStatistic/Platforms/Android/Services/FileSystemService.cs
git commit -m "feat: select Android document folders"
```

### Task 5: Connect folder selection to the export screen

**Files:**
- Modify: `TinkoffInvestStatistic/TinkoffInvestStatistic/ViewModels/ExportViewModel.cs`
- Modify: `TinkoffInvestStatistic/TinkoffInvestStatistic/Views/ExportPage.xaml`

- [ ] **Step 1: Simplify ExportViewModel state and commands**

In `ExportViewModel`:

- Remove `SettingsImportPath`, `DataImportPath`, `TransfersImportPath` and their backing fields.
- Remove `PickSettingsFileCommand`, `PickDataFileCommand`, `PickTransfersFileCommand`.
- Remove `IFileSystemService`, `GetImportExportFolder`, and `PickImportFileAsync`.
- Add `private readonly IExportFolderService _folderService;`.
- Resolve it in the constructor and throw a clear exception if it is unavailable:

```csharp
_folderService = DependencyService.Get<IExportFolderService>()
    ?? throw new InvalidOperationException("Сервис выбора папки недоступен.");
```

- [ ] **Step 2: Make ExportAsync choose a folder and report filenames**

Replace the path portion of `ExportAsync` with:

```csharp
using var cancelTokenSource = new CancellationTokenSource();
var cancellation = cancelTokenSource.Token;
var folderId = await _folderService.PickFolderAsync(cancellation);
if (folderId == null)
{
    return;
}

var result = await _exportService.ExportAsync(exportCategories, folderId, cancellation);
await _messageService.ShowAsync(
    "Сохранены файлы:\n" + string.Join("\n", result.SavedFiles));
```

Keep the existing category validation, authentication, exception handling, and `finally` block.

- [ ] **Step 3: Make ImportAsync choose a folder and report missing categories**

Replace the path and explicit-file portion of `ImportAsync` with:

```csharp
using var cancelTokenSource = new CancellationTokenSource();
var cancellation = cancelTokenSource.Token;
var folderId = await _folderService.PickFolderAsync(cancellation);
if (folderId == null)
{
    return;
}

var result = await _exportService.ImportAsync(importCategories, folderId, cancellation);
if (result.ImportedFiles.Count == 0)
{
    await _messageService.ShowAsync("В выбранной папке подходящие файлы не найдены.");
    return;
}

var message = "Импортированы файлы:\n" + string.Join("\n", result.ImportedFiles);
if (result.MissingCategories.Count > 0)
{
    message += "\n\nНе найдены: " + string.Join(
        ", ",
        result.MissingCategories.Select(GetCategoryName));
}

await _messageService.ShowAsync(message);
```

Add `using System.Linq;` and this helper:

```csharp
private static string GetCategoryName(ExportCategories category)
{
    return category switch
    {
        ExportCategories.Settings => "настройки",
        ExportCategories.Data => "данные",
        ExportCategories.Transfers => "зачисления",
        _ => category.ToString(),
    };
}
```

- [ ] **Step 4: Simplify the XAML layout**

In `ExportPage.xaml`:

- Rename labels from `Экспорт настроек:`, `Экспорт данных:`, and `Экспорт зачислений:` to `Настройки:`, `Данные:`, and `Зачисления:`.
- Delete the three `Выбрать файл ...` buttons and their three bound path labels.
- Change the grid rows to contain the three category rows, a flexible spacer, and the two action buttons.
- Preserve existing theme colors, button styles, bindings to `ExportCommand` and `ImportCommand`, and the `RefreshView`.

- [ ] **Step 5: Compile XAML and the view model**

Run:

```powershell
dotnet build TinkoffInvestStatistic\TinkoffInvestStatistic\TinkoffInvestStatistic.csproj -c Debug --no-restore
```

Expected: build succeeds with zero errors and no XAML binding warnings for removed properties.

- [ ] **Step 6: Commit the screen integration**

```powershell
git add TinkoffInvestStatistic/TinkoffInvestStatistic/ViewModels/ExportViewModel.cs TinkoffInvestStatistic/TinkoffInvestStatistic/Views/ExportPage.xaml
git commit -m "feat: choose folder for import and export"
```

### Task 6: Remove broad storage permission and verify the feature

**Files:**
- Modify: `TinkoffInvestStatistic/TinkoffInvestStatistic/Platforms/Android/AndroidManifest.xml`
- Create: `Tests/Tests/Application/AndroidStorageConfigurationTests.cs`

- [ ] **Step 1: Add a failing manifest test**

Create `Tests/Tests/Application/AndroidStorageConfigurationTests.cs` using the same repository-root helper pattern as `AndroidNetworkSecurityConfigurationTests`:

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Tests.Application;

[TestClass]
public class AndroidStorageConfigurationTests
{
    private const string AndroidNamespace = "http://schemas.android.com/apk/res/android";

    [TestMethod]
    public void Manifest_DoesNotRequestBroadExternalStoragePermission()
    {
        var manifest = XDocument.Load(GetAndroidManifest());
        var permissions = manifest.Root?
            .Elements("uses-permission")
            .Select(element => element.Attribute(XName.Get("name", AndroidNamespace))?.Value)
            .ToArray();

        CollectionAssert.DoesNotContain(
            permissions,
            "android.permission.WRITE_EXTERNAL_STORAGE");
        CollectionAssert.DoesNotContain(
            permissions,
            "android.permission.MANAGE_EXTERNAL_STORAGE");
    }

    private static string GetAndroidManifest([CallerFilePath] string sourceFilePath = "")
    {
        var repositoryRoot = Path.GetFullPath(Path.Combine(
            Path.GetDirectoryName(sourceFilePath)!,
            "..",
            "..",
            ".."));
        return Path.Combine(
            repositoryRoot,
            "TinkoffInvestStatistic",
            "TinkoffInvestStatistic",
            "Platforms",
            "Android",
            "AndroidManifest.xml");
    }
}
```

- [ ] **Step 2: Run the test and verify that it fails**

Run:

```powershell
dotnet test Tests\Tests\Tests.csproj --filter FullyQualifiedName~AndroidStorageConfigurationTests
```

Expected: the assertion fails because `WRITE_EXTERNAL_STORAGE` is currently present.

- [ ] **Step 3: Remove only the obsolete permission**

Delete this exact line from `Platforms/Android/AndroidManifest.xml`:

```xml
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
```

Preserve `android:networkSecurityConfig`, the bundled-certificate configuration, and every unrelated permission already present in the user's working tree.

- [ ] **Step 4: Run automated verification**

Run:

```powershell
dotnet test Tests\Tests\Tests.csproj
dotnet build TinkoffInvestStatistic\TinkoffInvestStatistic\TinkoffInvestStatistic.csproj -c Release --no-restore
git diff --check
```

Expected: all tests pass, the Android Release build succeeds with zero errors, and `git diff --check` prints nothing. Record existing package vulnerability warnings separately; do not expand this feature to dependency upgrades.

- [ ] **Step 5: Perform device verification on Huawei**

Install the newly signed Release APK, then verify this exact sequence:

1. Open the export screen, select all three categories, tap `Экспортировать`, and choose a visible folder such as `Documents/TinkoffInvestStatistic`.
2. Confirm that exactly three files dated today appear in the Huawei file manager.
3. Export again on the same day and confirm that the same three names are replaced, not duplicated.
4. Change a setting in the app, tap `Импортировать`, select the same folder, and confirm that the newest settings file restores the prior value.
5. Remove one category file, import all categories, and confirm that the remaining files import while the missing category is listed.
6. Cancel both folder pickers and confirm that no error appears and no data changes.
7. Place malformed XML under a valid filename and confirm that the app reports that filename without clearing existing data.

- [ ] **Step 6: Commit permission removal and verification test**

```powershell
git add TinkoffInvestStatistic/TinkoffInvestStatistic/Platforms/Android/AndroidManifest.xml Tests/Tests/Application/AndroidStorageConfigurationTests.cs
git commit -m "test: verify scoped Android folder storage"
```

## Final verification checklist

- [ ] `ExportCategories` values are `1`, `2`, and `4`.
- [ ] No code combines `content://` identifiers with `Path.Combine`, `Directory`, or `File` APIs.
- [ ] Export replaces same-day names and returns the filenames shown to the user.
- [ ] Import chooses by date embedded in the filename, then `LastModified`, then stable URI order.
- [ ] Missing categories do not block valid categories.
- [ ] Malformed XML is deserialized before category data is modified.
- [ ] Folder cancellation is a no-op.
- [ ] `WRITE_EXTERNAL_STORAGE` and `MANAGE_EXTERNAL_STORAGE` are absent.
- [ ] The full MSTest suite and Android Release build pass.
- [ ] Only task-specific files are staged in each commit; do not use `git commit -a` because the working tree already contains unrelated prepared changes.

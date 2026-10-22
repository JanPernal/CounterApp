# CounterApp – roadmap

Last updated: 2026-10-08. `[x]` = done, `[ ]` = to do.

## Done

### Stage 0: fundamentals
- [x] C# basics, classes and objects
- [x] MAUI installed, first app built
- [x] Git repo + `.gitignore`

### Grade 2: one counter (code-behind) — commit `a319b78`
- [x] XAML page with `+` / `-` buttons and a label
- [x] `Clicked` handlers updating the label by hand

### Refactor 1: MVVM — commit `2168cb4`
- [x] CommunityToolkit.Mvvm installed
- [x] `CounterViewModel` : `ObservableObject` with `[ObservableProperty]` (Value, Name)
- [x] `[RelayCommand]` Increment / Decrement / Reset
- [x] Reset disabled at initial value (`CanExecute` + `NotifyCanExecuteChangedFor`)
- [x] `CounterPage.xaml` uses `{Binding}` + `x:DataType`, code-behind is empty
- [x] Namespaces match folders (`CounterApp.ViewModels`)
- [x] `await Navigation.PushAsync(...)` in `async void` handlers

### Grade 3: many counters with a name — commits `2e25f32`, `cf424b3`
- [x] `MainViewModel` with `Counters` (`ObservableCollection`), `NewCounterName`, `AddCounterCommand`
- [x] `MainPage.xaml`: `Grid` layout (`Auto,Auto,*`), `Entry`, `CollectionView` + `DataTemplate`
- [x] "New Counter" disabled while the name is empty (`CanExecute`)
- [x] `CounterPage` deleted, history cleaned and pushed
- [x] `CanAdd` → `!string.IsNullOrWhiteSpace(...)`

### Grade 4 — commits `18b8d5e`, `fe4a362`: save state + load it on app start (format is free; using XML / XDocument)
Official requirement: counters are saved, and the saved state is loaded when the app starts.
- [x] `Counter` model in `Models/` (plain data: Id, Name, Value, InitialValue, ColorHex)
- [x] `ICounterStorage` interface with `Task<List<Counter>> LoadAsync()` and `Task SaveAsync(IEnumerable<Counter>)`
- [x] `XmlCounterStorage` using `XDocument` + file in `FileSystem.AppDataDirectory`
- [x] Handle first start: file does not exist yet → return an empty list
- [x] Mapping `Counter` ↔ `CounterViewModel` (both directions)
- [x] Register storage, `MainViewModel` and `MainPage` in `MauiProgram.cs` (dependency injection)
- [x] `MainPage` gets `MainViewModel` through its constructor instead of `new`
- [x] Load counters on start (`OnAppearing` → `await LoadAsync()`), save with a "Save" button (`async Task` command)
- [x] Test: add counters, save, close the app fully, start again → counters are back
- [x] Cleanup: remove `Debug.WriteLine` in `SaveAsync` and the commented-out Load button
- [x] Commit + push

Reading: interfaces in C#, async/await, LINQ to XML (`XDocument`, `XElement`), File system helpers, Dependency injection in MAUI.

## In progress

### Grade 5: autosave and initial value
- [x] Autosave whenever a counter changes (`partial void OnValueChanged`)
- [x] Initial value chosen when creating a counter; new counter starts at it
- [x] `InitialValue` becomes an `[ObservableProperty]` (so Reset's `CanExecute` stays correct)
- [x] Test: new counter with initial value 5 starts at 5, Reset disabled until it changes, survives restart
- [x] Check: negative initial value on iPhone numeric keypad → no minus key
- [ ] Initial value as `string` + `int.TryParse` (empty = 0, invalid → New Counter disabled), default keyboard
- [ ] Commit + push
- [ ] Optional: creation form as a separate page (Shell navigation, `GoToAsync`)

Reading: `[ObservableProperty]` → `On...Changed` partial methods, Shell navigation.

## To do

### Grade 6: color, data storage, reset, delete
- [ ] Color per counter: value converter string → `Color` (`Color.FromArgb`)
- [ ] Data stored in a "more convenient" format (official: XML/XDocument or JSON) — XML is already done in Grade 4; optional extra: `JsonCounterStorage` swapped in via DI to show off the interface
- [ ] Check the official task: is SQLite required, or was it only in the reading list?
- [ ] Delete a counter, with `DisplayAlert` confirmation
- [x] Reset to initial value (already done in Refactor 1)

Reading: local SQLite database in MAUI, value converters, colors in MAUI, pop-ups.

## Extra materials
- .NET MAUI Workshop (official, step by step: MVVM, CollectionView, navigation)
- dotnet/maui-samples
- Enterprise Application Patterns using .NET MAUI (MVVM and DI chapters for now)

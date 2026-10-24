# CounterApp – roadmap

Last updated: 2026-10-23. `[x]` = done, `[ ]` = to do.

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

### Grade 5: autosave and initial value — commits `f9f7891`, `50ee711`
- [x] Autosave whenever a counter changes (`partial void OnValueChanged`)
- [x] Initial value chosen when creating a counter; new counter starts at it
- [x] `InitialValue` becomes an `[ObservableProperty]` (so Reset's `CanExecute` stays correct)
- [x] Test: new counter with initial value 5 starts at 5, Reset disabled until it changes, survives restart
- [x] Check: negative initial value on iPhone numeric keypad → no minus key
- [x] Initial value as `string` + `int.TryParse` (empty = 0, invalid → New Counter disabled), default keyboard
- [x] Commit + push
- [ ] Optional (later): creation form as a separate page (Shell navigation, `GoToAsync`)

## In progress

### Grade 6: color, data storage, reset, delete
- [x] Reset to initial value (already done in Refactor 1)
- [x] Data stored in a "more convenient" format: XML / XDocument (done in Grade 4)
- [ ] Check the official task: is SQLite required, or was it only in the reading list?

Step 1: delete a counter
- [x] `[RelayCommand] async Task DeleteCounter(CounterViewModel counter)` in `MainViewModel`
- [x] Confirmation with `DisplayAlertAsync` (returns `bool`; `DisplayAlert` is obsolete in .NET 10)
- [x] Unsubscribe `counter.Changed -= OnCounterChanged`, remove from `Counters`, save
- [x] "Delete" button in the `DataTemplate`, bound to the parent view model (`RelativeSource AncestorType`) with `CommandParameter="{Binding .}"`
- [ ] Test: delete, restart → still gone; cancel → nothing happens

Step 2: color per counter
Design decision: a fixed palette of ~20 colors (same saturation + brightness, only hue changes). Each counter stores its base color as `ColorHex` (no model/XML change). Light/dark variants for buttons and icons are computed from it by the converter.
- [x] 2a. `Converters/HexToColorConverter.cs` (`IValueConverter`, `Color.FromArgb`, fallback for empty/invalid text, `ConvertBack` not supported)
- [x] 2b. Register it in `<ContentPage.Resources>` and use it to color each counter in the list
- [x] 2c. `ConverterParameter` variants: `Light` (button background) and `Dark` (icon), rebuilt from the hue with `Color.FromHsla` (Light S70 L85, Dark S100 L24)
- [ ] 2d. `Models/ColorPalette.cs`: static list of 20 base hex colors, `Color.FromHsla(i / 20.0, 1.0, 0.45)` → `ToArgbHex()`
- [ ] 2e. `MainViewModel`: `AvailableColors` (from the palette), `NewColorHex`, `SelectColor(string hex)` command, `AddCounter` copies `NewColorHex`
- [ ] 2f. Swatch grid in the form (`FlexLayout` wrap + `BindableLayout`), selected color visibly marked
- [ ] Test: new counters get the chosen color, colors survive restart

Later: new UI (from the mockup)
- [ ] Card per counter: `Border` with rounded corners, color dot + name, big value (`StringFormat='{0:N0}'` → 1,100)
- [ ] `-` button in the light variant with a dark icon, `+` button in the base color with a white icon
- [ ] `SwipeView`: swipe left → Delete (red, trash icon), swipe right → Reset (grey, reset icon)
- [ ] Probably: creation form on its own page (Shell navigation)

Step 3 (optional extra)
- [ ] `JsonCounterStorage` swapped in via DI (one line in `MauiProgram.cs`) to show off the interface

Reading: commands with parameters, `RelativeSource` bindings, pop-ups (`DisplayAlertAsync`), value converters, colors in MAUI, local SQLite database in MAUI.

## Extra materials
- .NET MAUI Workshop (official, step by step: MVVM, CollectionView, navigation)
- dotnet/maui-samples
- Enterprise Application Patterns using .NET MAUI (MVVM and DI chapters for now)

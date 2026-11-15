# Calendar App (C# / Windows Forms)

A desktop calendar for Windows, written in C# with Windows Forms (.NET Framework 4.8). It started as a university project for an object-oriented programming course. You can add events and look at them by day, week, month or year.

## Features

- **Four views:** day (24-hour timeline), week (Monday to Sunday), month grid and year overview. Days that have events are highlighted in the year view.
- **Mini calendar:** pick a date on the left and the open view jumps to that date.
- **Events:** each event has a name, start and end time, and a note. Add and edit them in a dialog window. If a day has several events, you choose which one to edit.
- **Event details:** click an event to open its details, and from there mark it as done (it turns grey with a tick) or delete it.
- **Validation:** an event needs a name and cannot end before it starts.

## Project structure

| File | What it does |
|------|--------------|
| `Event.cs` | The `Event` class: id, name, start, end, note, done flag |
| `Manager.cs` | Keeps the list of events, keeps it sorted, and answers queries by day, month or year using LINQ |
| `Form1.cs` | Main window: view buttons, mini calendar, switching views. It owns the single `Manager` that all views share |
| `AddEvent.cs` | Dialog for adding and editing an event, with input validation |
| `details.cs` | Dialog showing one event, with "mark as done" and "delete" |
| `ViewDay.cs`, `ViewWeek.cs`, `ViewMonth.cs`, `ViewYear.cs` | One `UserControl` per view, each with a `Populate` method that draws the events |

## What I practised

- Object-oriented design: separate classes for data (`Event`) and logic (`Manager`)
- Reusable `UserControl` views swapped into one main panel
- Sharing one object between several forms and controls, so they all work on the same data
- LINQ for filtering and sorting events
- Building layouts in code with `TableLayoutPanel` and `FlowLayoutPanel`

## How to run

1. Install Visual Studio with the ".NET desktop development" workload.
2. Open `calendarapp.slnx`.
3. Press **F5**.

## Screenshots

<!-- Add 1-2 screenshots here, for example the week view and the month view. -->

## Known limitations

This is a student project and still a work in progress.

- Events are kept in memory only, so they are lost when the app is closed. There is no database or file saving yet.
- Events are shown in the hour they start. Two events that start in the same hour overlap in the day and week views.
- Events that last past midnight are shown only until the end of their first day.

## Possible next steps

- Save events to a file or a SQLite database
- Show overlapping events side by side
- Unit tests for `Manager`

# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| R5 | Services/BookingService.cs | AI-assisted code review | Added validation using TimeProvider to reject bookings in the past |
| R6 | Services/BookingService.cs | AI-assisted code review | Added validation for the maximum booking duration of 4 hours |
| R7 | Services/BookingService.cs | AI-assisted code review | Changed the overlap condition to allow touching intervals
| R7 | Services/BookingService.cs | AI-assisted code review | Updated the overlap check to consider only active bookings |
| R8 | Services/BookingService.cs | AI-assisted code review | Added conflict handling when cancelling an already cancelled booking
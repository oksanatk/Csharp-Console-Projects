# Habit Logger Console App

Console-based app that uses CRUD operations against an SQLite database to track user habits.

### Users can:
- [x] Create a new habit
- [x] Add, update, or delete records to existing habits
- [x] View all records in a habit in a table. Includes record id, quantity recorded, and the date.
- [x] View various statistics in a Report option in the main menu.

### Features:
- [x] Speech-to-Text Voice Recognition mode using Azure's Mircosoft.CognitiveServices.Speech library
- [x] paramaterized queries (where possible) and input-validation where not mitigate the risk of sql-injections
- [x] no known reasons for the app to crash currently

### How to Use
  - Build and run as a dotnet application
  - use `--voice-input` command-line argument for Voice-Recognition mode

## Challenges / Lessons Learned:
- new to sql and sqlite
- ms ado.net SQLite ["hello world" documentation](https://github.com/dotnet/docs/blob/main/samples/snippets/standard/data/sqlite/HelloWorldSample/Program.cs) 
        was out-of-date. The examples used .net 6, not .net 8
        -> and didn't release the file after I created / edited it for deletion. 
        -> now, I know to clear .ClearAllPools() to release the file handles. 
      Hours of frustration later taught me about file handles, and I contributed a single line of code to the official MS docs to keep them up-to-date.
- familiar with the idea of classes to represent "objects" (like a calculator), 
        but conceptually new to using oop for things like creating a connection layer, etc. 


## Areas To Improve:
- Plan Ahead!
        * Many functions mix getting user inputs and interaction with SqliteConnections to the database. 
            separating these into different methods and different classes from the beginning, rather than at the end as an afterthought
              would have helped with reducing repeat code a LOT.
            ** did start implementing optional parameters to help with date validation towards the end, but it would've saved me 
                a lot of time to plan for these types of issues from the beginning.
- Could I have used enumeration to simplify? ie, enumerate either menu options or sqlite queries until methods like:
            * create / edit / delete a habit become one main HabitInteraction() method with different,
                                                  potentially optional parameters that call more specific methods when needed?
- Are there better ways of testing?
            * I still largely debug by running the app and seeing what pops up on the screen. I have now leveled up to 
              being comfortable with using breaklines, looking at local variables, and stepping over / into a function 
              when debugging. But what else can help me improve?
              
              + some of these options weren't available for checking if my sql queries made sense or had syntax issues. 
              first question of any new concept is how do I debug / get unstuck / look at or interact with this thing I'm using?
              
- Branch problems / features
      * as opposed to just making commits when I'm done coding for the day / done with a major method / section of code.
      * branching off of master could make it easier to review the development process later + is more realistic.

## Resources used:
- sqlite tutorial [here](https://www.youtube.com/watch?v=HQKwgk6XkIA)
- MS [ADO.Net docs](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-overview)
- [sqlite docs](https://www.sqlite.org/docs.html)
- chatgpt for answering dumb questions. especially about sqlite syntax issues.
  
  

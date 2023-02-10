# ToDoListApp
Downloaded from GitHub at https://github.com/niritgit/ToDoListApp.git
Run the ToDoListApp.
Go to Postman (or another tool):
****For get tasks:****
Add request - 
action method: GET
Url: http://localhost:{PUT HERE SUITABLE PORT FOR APP}/api/tasks?pageNumber=3&pageSize=2
(you can change pageNumber and pageSize)

***For create tasks:***
Add request - 
action method: POST
Url: http://localhost:{PUT HERE SUITABLE PORT FOR APP}/api/tasks/CreateTask
Headers:
  Content-type: application/json
Body: raw
write tasks there, for example:
[{
    "Title": "New qa2 To Do",
    "Description":"desc for qa",
    "status": "notStarted",
    "DueDate": "2023-02-20",
    "CreatedAt": "2023-02-20",
    "UpdatedAt": "2023-02-20"
  },
  {
    "Title": "New grafh To Do",
    "Description":"desc for graph to do",
    "status": "InProcess",
    "DueDate": "2023-03-20",
    "CreatedAt": "2023-02-20",
    "UpdatedAt": "2023-02-20"
  }]
 ****For update tasks:***
 Add request - 
action method: PUT
Url: http://localhost:{PUT HERE SUITABLE PORT FOR APP}/api/tasks/UpdateTask
Headers:
  Content-type: application/json
Body: raw
write tasks with changes here (tasks ids should be exist in db), for example:
[{   
    "Id": 1,
    "Title": "New PR To Do",
    "Description":"pr1",
    "status": "inProcess",
    "DueDate": "2023-03-20",
    "CreatedAt": "2023-02-20",
    "UpdatedAt": "2023-03-10"
  },
  {
      "Id": 2,
    "Title": "New Test To Do",
    "Description":"qqq1",
    "status": "Done",
    "DueDate": "2023-03-20",
    "CreatedAt": "2023-02-20",
    "UpdatedAt": "2023-02-20"
  }
  ]
****For delete tasks:***
Add request - 
action method: DELETE
Url: http://localhost:{PUT HERE SUITABLE PORT FOR APP}/api/tasks/DeleteTask
Headers:
  Content-type: application/json
Body: raw
write tasks TO BE DELETED here (tasks ids should be exist in db), for example:
[{"Id":1,
    "Title": "New qa1 To Do",
    "Description":"qa1",
    "status": "notStarted",
    "DueDate": "2023-03-20",
    "CreatedAt": "2023-02-20",
    "UpdatedAt": "2023-02-20"
  },
  {
      "Id":2,
    "Title": "New grafh1 To Do",
    "Description":"VVV",
    "status": "notStarted",
    "DueDate": "2023-03-20",
    "CreatedAt": "2023-02-20",
    "UpdatedAt": "2023-02-20"
  }]

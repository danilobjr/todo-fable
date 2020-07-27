module State

open System
open Todo

type Filter =
    | All
    | Active
    | Completed

type State = {
    AllCompleted: bool
    Filter: Filter
    Todos: Todo list }

type Action =
    | Add of string
    | ClearCompleted
    | Filter of Filter
    | Remove of Guid
    | ToggleCompleted of Guid
    | SetAllAsCompleted of bool

let initialState = {
    AllCompleted = false
    Filter = All
    Todos = Data.data }

let filterTodos filter (todos: Todo list) =
    match filter with
    | Active ->
        todos |> List.filter (fun t -> not t.Completed)
    | Completed ->
        todos |> List.filter (fun t -> t.Completed)
    | All ->
        todos

let reducer state action =
    match action with
    | Add text ->
        // FIXME should only create a new todo if text is not empty
        // Put this logic inside domain model
        let newTodo = create text
        { state with Todos = newTodo::state.Todos }
    | ClearCompleted ->
        let activeTodos =
            state.Todos
            |> filterTodos Active
        { state with Todos = activeTodos }
    | Filter Active ->
        { state with Filter = Active }
    | Filter Completed ->
        { state with Filter = Completed }
    | Filter All ->
        { state with Filter = All }
    | Remove id ->
        let todos =
            state.Todos
            |> List.filter (fun t -> t.Id <> id)
        { state with Todos = todos }
    | ToggleCompleted id ->
        let todos =
            state.Todos
            |> List.map (fun t ->
                if (t.Id = id) then
                    { t with Completed = not t.Completed }
                else
                    t)
        { state with Todos = todos }
    | SetAllAsCompleted completed ->
        let todos =
            state.Todos
            |> List.map (fun t -> { t with Completed = completed })
        { state with
            AllCompleted = completed
            Todos = todos }
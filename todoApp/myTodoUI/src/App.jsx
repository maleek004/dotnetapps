import { useState, useEffect } from 'react';

function App() {
  // 1. State Variables
  const [todos, setTodos] = useState([]);
  const [newTitle, setNewTitle] = useState("");

  // API Base URL
  const API_URL = "http://localhost:5000/todos";

  // 2. Fetch Todos from Backend
  const fetchTodos = async () => {
    try {
      const response = await fetch(API_URL);
      const data = await response.json();
      setTodos(data);
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  };

  // 3. Trigger Fetch on Load
  useEffect(() => {
    fetchTodos();
  }, []);

  // 4. Add a New Todo
  const addTodo = async (e) => {
    e.preventDefault();
    if (!newTitle.trim()) return;

    try {
      const response = await fetch(API_URL, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ title: newTitle, isCompleted: false })
      });

      if (response.ok) {
        setNewTitle(""); // Clear the input box
        fetchTodos();    // Refresh the list from the database
      }
    } catch (error) {
      console.error("Error adding todo:", error);
    }
  };

  // 5. Delete a Todo
  const deleteTodo = async (id) => {
    try {
      const response = await fetch(`${API_URL}/${id}`, {
        method: "DELETE"
      });

      if (response.ok) {
        fetchTodos(); // Refresh the list from the database
      }
    } catch (error) {
      console.error("Error deleting todo:", error);
    }
  };

  // 6. The HTML Layout (JSX)
  return (
    <div style={{ padding: '20px', fontFamily: 'sans-serif' }}>
      <h1>Todo List</h1>
      
      {/* Form to Add Todo */}
      <form onSubmit={addTodo}>
        <input 
          type="text" 
          placeholder="Add a new task..." 
          value={newTitle} 
          onChange={(e) => setNewTitle(e.target.value)} 
        />
        <button type="submit">Add Task</button>
      </form>

      {/* List of Todos */}
      <ul>
        {todos.map((todo) => (
          <li key={todo.id} style={{ margin: '10px 0' }}>
            {todo.title} 
            <button 
              onClick={() => deleteTodo(todo.id)} 
              style={{ marginLeft: '10px', color: 'red' }}
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
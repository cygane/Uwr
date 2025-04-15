let todos = [
    { text: "Buy milk", done: false }, 
    { text: "Go to the gym", done: true }, 
    { text: "Read a book", done: false },
    { text: "Write code", done: true },
    { text: "Walk the dog", done: false },
    { text: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vel metus sed justo vehicula rutrum vel ut est. Maecenas a dictum nunc. Proin congue libero risus, et lobortis purus venenatis eu. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Morbi pulvinar ullamcorper tortor, quis cursus quam.", done: false },
];

function render(){
    const list = document.getElementById("todo-list");
    list.innerHTML = "";
  
    todos.forEach((task, index) => {
      const li = document.createElement("li");
      li.className = "todo__container";
      if(task.done){
        li.classList.add("todo__container--completed");
      }
      
      const innerHTML = `
        <div class="todo-element todo-name">${task.text}</div>
        <button class="todo-element todo-button move-up">↑</button>
        <button class="todo-element todo-button move-down">↓</button>
        <button class="todo-element todo-button">${
          task.done ? "Revert" : "Done"
        }</button>
        <button class="todo-element todo-button">Remove</button>
      `;
      li.innerHTML = innerHTML;
  
      const buttons = li.querySelectorAll("button");
  
      buttons[0].onclick = () => toggleUp(index);
      buttons[1].onclick = () => toggleDown(index);
      buttons[2].onclick = () => toggleDoneOrRevert(index);
      buttons[3].onclick = () => deleteTask(index);
  
      list.appendChild(li);
    });
  
    document.getElementById("count").textContent = count();
}
  

function toggleDoneOrRevert(index){
    todos[index].done = !todos[index].done;
    render();
}

function toggleUp(index){
    if(index > 0){
        var toUp = todos[index];
        todos[index] = todos[index - 1];
        todos[index - 1] = toUp;
        render();
    }
}

function toggleDown(index){
    if(index < todos.length - 1){
        var toDown = todos[index];
        todos[index] = todos[index + 1];
        todos[index + 1] = toDown;
        render();
    }
}

function deleteTask(index){
    todos.splice(index, 1);
    render();
}

function addTask(taskName){
    if(taskName){
        todos.push({text: taskName, done: false});
        render();
    }
}

function clearAll(){
    todos = [];
    render();
}

function count(){
    return todos.filter((task) => !task.done).length;
}

render();

document.getElementById("add-todo-form").addEventListener("submit", function(e) {
    e.preventDefault();
    const input = e.target.querySelector("input[name='todo-name']");
    const value = input.value.trim();
    if (value) {
      addTask(value);
      input.value = "";
    }
});
  

document.getElementById("todos-clear").addEventListener("click", clearAll);

import './App.css';

function App() {

  return (
    <div className="App">
      <form>
        {/*
        <label htmlFor="book-title">
          Book Title
        </label>
        <input type="text" id="book-title"></input>

        <label htmlFor="author">
          Books Author
        </label>
    <input type="text" id="author"></input>
    <input type="submit" value="Get the results" onClick={getBooksResults}></input>*/}
    <input type="submit" value="Get all books" onClick={getAllBooks}></input>
      </form>
    </div>
  );
}

function getAllBooks(event)
{
  event.preventDefault();
  const response = fetch("https://localhost:7235/api/v1/books");
}

function getBooksResults(event)
{
  event.preventDefault();
}

export default App;

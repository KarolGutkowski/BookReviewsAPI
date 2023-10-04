import './App.css';
import { useState } from 'react';
import Book from "./Components/Book"

function App() {
  const [books,setBooks] = useState([]);

  async function getAllBooks(event)
  {
    event.preventDefault();
    try{
      const response = await fetch("https://localhost:7235/api/v1/books");
      const json = await response.json();
      setBooks(json);
    }catch(error)
    {
      console.error("failed loading books");
    }
  }
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
    <div>
      {
        !books.length?(
          <p>No books returned</p>
        ):
        (
          books.map((book)=>{
              return <Book key={book.id} props={book}/>
            })
        )
      }
     </div> 
      
      </form>
    </div>
  );
}

/*
function getBooksResults(event)
{
  event.preventDefault();
}*/

export default App;

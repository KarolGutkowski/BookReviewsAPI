import React from "react";
import { useState } from "react";
import Book from "./Book";

export default function BooksQueryForm()
{
    const [books,setBooks] = useState([]);
    async function getAllBooks(event)
    {
        event.preventDefault();
        try{
            const response = await fetch("https://localhost:7235/api/v1/books",
            {
              credentials: "include",
            });
            const json = await response.json();
            setBooks(json);
        }catch(error)
        {
            console.error("failed loading books");
        }
    }



  return (
    <div className="App">
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
    </div>
  );
}
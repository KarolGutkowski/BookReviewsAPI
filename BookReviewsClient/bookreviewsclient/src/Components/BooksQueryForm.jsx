import React from "react";
import { useState } from "react";
import Book from "./Book";
import {config} from "../Constants"

export default function BooksQueryForm()
{
    const [books,setBooks] = useState([]);
    async function getAllBooks(event)
    {
        event.preventDefault();
        try{
            const response = await fetch(`${config.url}/api/v1/books`,
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
    <div>
      {
      books.length===0? 
      <div className="get-all-books-container">
        <button className="get-all-books-button" onClick={getAllBooks}>All books</button>
      </div>:
      null
      }
    <div className="books-container">
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
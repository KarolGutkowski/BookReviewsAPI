import React from "react";
import { useState, useEffect } from "react";
import Book from "./Book";
import {config} from "../Constants"

export default function BooksQueryForm()
{
    const [books,setBooks] = useState([]);

    useEffect(()=>
    {
      async function getAllBooks()
      {
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

      getAllBooks()
      .catch(err =>
        {
          console.error(err);
        })

    },[])


    

  return (
    <div>
    <div className="books-container">
      {
        !books.length?(
          <p>No books loaded</p>
        ):
        (
          books.map((book)=>{
              return <Book key={book.id} book={book}/>
            })
        )
      }
     </div> 
    </div>
  );
}
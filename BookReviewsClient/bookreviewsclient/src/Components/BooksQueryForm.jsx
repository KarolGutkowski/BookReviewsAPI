import React from "react";
import { useState, useEffect } from "react";
import Book from "./Book";
import {config} from "../Constants"

export default function BooksQueryForm()
{
    const [books,setBooks] = useState([]);
    const [pageNumber, setPageNumber] = useState(1);

    useEffect(()=>
    {
      async function getAllBooks()
      {
          try{
              const response = await fetch(`${config.url}/api/v1/books?` + new URLSearchParams({
                pageNumber: pageNumber
              }),
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

    },[pageNumber])


    

  return (
    <div>
    <div className="books-container">
      {
        !books.length?(
          <div className="loader" id="loader-7"></div>
        ):
        (
          books.map((book)=>{
              return <Book key={book.id} book={book}/>
            })
        )
      }
      <p onClick={()=>setPageNumber(pageNumber-1)}>Previous page</p>
      <p onClick={()=>setPageNumber(pageNumber+1)}>Next page</p>
     </div> 
    </div>
  );
}
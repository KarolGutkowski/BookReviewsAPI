import Book from "./Book";
import { useNavigate } from "react-router-dom";

export default function BooksSearchResults({books})
{
    const navigate = useNavigate();
    
    if(!books)
        navigate("/");

    return (
        <div>
        <div className="books-container">
            {
                !books || !books.length || books.length === 0?
                <p>Couldn't find any mathcing books 😞</p>:
                <p>Found {books.length} book{books.length===1? null: 's'}</p>
            }
            {
                !books ? 
                null: 
                books.map((book)=>{
                return <Book key={book.id} book={book}/>
                })
            }
            
        </div> 
        </div>
    );
}
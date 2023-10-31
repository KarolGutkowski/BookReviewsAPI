import { useEffect, useContext } from "react";
import UserLoginStateContext from "./UserLoginStateContext";
import { useParams } from "react-router-dom";
import { config } from "../Constants"
import { useState } from "react";

const BookDetails = () =>
{
    const {id} = useParams();
    const [book,setBook] = useState(null);

    useEffect(()=>
    {
        fetch(`${config.url}/api/v1/books/${id}`,
            {
                method: "GET",
                credentials: "include",
            })
        .then(response =>
            {
                if(response.status === 200)
                {
                    return response.json()
                }
                return null
            })
        .then(data => 
            {
                setBook(data);
            })
        .catch(()=>
        {
            console.error("failed loading books");
        });
    
    },[]);


    return (
        book?
        <div className="book-details-page-container">
            <img className="large-book-cover" src={book.img} alt="book cover"></img>

            <div className="book-details-side-section">
                <div className="book-details-element-normal">
                    <p className="book-details-small-text">Title</p>
                    <p className="book-details-large-text">{book.title}</p>
                </div>

                <div className="book-details-element-normal">
                    <p className="book-details-small-text">Author</p>
                    <p className="book-details-large-text">{book.authors[0].firstName + " " + book.authors[0].lastName}</p>
                </div>

                <div className="book-details-element-normal">
                    <p className="book-details-small-text">Year</p>
                    <p className="book-details-large-text">{book.year}</p>
                </div>
            </div>
        </div>
        :
        null 
    )
}



export default BookDetails;
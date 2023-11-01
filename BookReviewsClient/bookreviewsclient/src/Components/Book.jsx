import { useContext, useEffect, useState } from "react";
import UserLoginStateContext from "./UserLoginStateContext";
import heartFilled from "../images/heart-filled-icon.png"
import heartEmpty from "../images/heart-empty-icon.png"
import {config} from "../Constants"
import {Link} from "react-router-dom"


const Book = (props)=>
{
    const {userName}= useContext(UserLoginStateContext);
    const [likedByUser, setLikedByUser] = useState(false);
    const {book} = props;
    const displayLikeImage = props.displayLikeImage??true;


    useEffect(()=>
    {
        if(userName)
        {
            const id = book.id;
            fetch(`${config.url}/api/v1/books/liked/${id}`,{
                                    method: "GET",
                                    credentials: "include",
            })
            .then((response)=>
            {
                if(response.status === 200)
                {
                    setLikedByUser(true);
                }else{
                    setLikedByUser(false)
                }
            })
            .catch((err) =>
            {
                console.log("Error while fetching data:" + err);
            })
        }
    },[userName]);

    function handleUserClickedHeart()
    {
        const id = book.id;
        if(likedByUser)
        {
            fetch(`${config.url}/api/v1/books/liked/remove/${id}`,{
                method: "PATCH",
                credentials: "include",
            }).then((response)=>{
                if(response.status === 200)
                {
                    setLikedByUser(false);
                }
            })
        }else
        {
            fetch(`${config.url}/api/v1/books/liked/${id}`,{
                method: "POST",
                credentials: "include"
            }).then((response)=>{
                if(response.status === 200)
                {
                    setLikedByUser(true);
                }
            })
        }
    }

    return (
            <div className="book-container">
                <div className="book-year-wrapper">
                    <p className="book-year">{book.year}</p>
                </div>
                <div className="book-images-container">
                    <Link to={`/book/${book.id}`}>
                        <img className="book-cover" src={book.img} alt="book cover"></img>
                    </Link>
                    {userName && displayLikeImage?
                    <img className="liked-book-icon" src={likedByUser?heartFilled:heartEmpty} alt="like book button" onClick={handleUserClickedHeart}></img>:
                    null
                    }
                </div>
            </div>
    );
}

export default Book;
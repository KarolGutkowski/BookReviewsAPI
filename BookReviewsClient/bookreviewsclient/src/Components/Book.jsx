import { useContext, useEffect, useState } from "react";
import UserLoginStateContext from "./UserLoginStateContext";
import heartFilled from "../images/heart-filled-icon.png"
import heartEmpty from "../images/heart-empty-icon.png"



const Book = ({props})=>
{
    const {userName}= useContext(UserLoginStateContext);
    const [likedByUser, setLikedByUser] = useState(false);

    useEffect(()=>
    {
        const id = props.id;
        fetch(`https://localhost:7235/api/v1/books/liked/${id}`,{
                                method: "GET",
                                credentials: "include",
        }).then((response)=>
        {
            if(response.status === 200)
            {
                setLikedByUser(true);
            }else{
                setLikedByUser(false)
            }
        })
    },[userName]);

    return (
        <div className="book-container">
            <p className={`book-title`}>{props.title}</p>
            <p>{props.year}</p>
            <img className="book-cover" src={props.img} alt="book cover"></img>
            {userName?
            <img className="liked-book-icon" src={likedByUser?heartFilled:heartEmpty} alt="like book button"></img>:
            null
            }
        </div>
    );
}

export default Book;
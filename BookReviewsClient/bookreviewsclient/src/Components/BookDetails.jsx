import { useEffect, useContext} from "react";
import UserLoginStateContext from "./UserLoginStateContext";
import { useParams } from "react-router-dom";
import { config } from "../Constants"
import { useState } from "react";
import BookReview from "./BookReview";
import BookAddReviewForm from "./BookAddReviewsForm";


const BookDetails = () =>
{
    const {id} = useParams();
    const [book,setBook] = useState(null);
    const [reviews, setReviews] = useState(null);
    const {user} = useContext(UserLoginStateContext);
    const [userHasReviewThisBook, setUserHasReviewedThisBook] = useState(false);

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
                    return response.json();
                }
                return null;
            })
        .then(data => 
            {
                setBook(data);
                if(data.reviews)
                {
                    setReviews(data.reviews);
                }
            })
        .catch(()=>
        {
            console.error("failed loading book");
        });

        if(user){
            fetch(`${config.url}/api/v1/reviews/book/${id}`,{
                method: "GET",
                credentials: "include",
            })
            .then(response =>
                {
                    if(response.status === 200)
                    {
                        return response.json();
                    }
                    return null;
                }).
            then(data => 
                {
                    if(data)
                    {
                        setUserHasReviewedThisBook(true);
                        console.log("user has already posted review on this book");
                    }
                })
            .catch(err => console.error("Some of your data might be unavailable"))
        }
    
    },[id, user]);
    
    return (
        book?
        <div className="book-details-page-container">
            <div className="book-details-section">
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
                    <div className="book-details-element-normal">
                        <div className="stars" style={{"--rating": book.averageRating}}></div>
                    </div>
                </div>
            </div>

            <div className="book-reviews-container">
                <h1>Reviews</h1>
                {user && !userHasReviewThisBook? <BookAddReviewForm bookId={book.id} setReviews={setReviews} setUserHasReviewedThisBook={setUserHasReviewedThisBook}/> :null }
                {!reviews || reviews.length === 0 ? 
                null: 
                reviews.map(review => <BookReview review={review}/>)
                }
            </div>
        </div>
        :
        null 
    )
}



export default BookDetails;
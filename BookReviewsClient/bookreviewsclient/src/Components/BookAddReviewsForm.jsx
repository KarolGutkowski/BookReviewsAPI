import { useContext } from "react";
import UserLoginStateContext from "./UserLoginStateContext";
import '../styles/reviews.css'
import { config } from "../Constants"
import moment from "moment";

const BookAddReviewForm = (props) =>
{
    const {user} = useContext(UserLoginStateContext);
    const {bookId, setReviews, setUserHasReviewedThisBook} = props;

    function handleStarsMove(event)
    {
        const target = event.target;
        const rect = target.getBoundingClientRect();
        const left = rect.left;
        const right = rect.right;
        const width = right - left;
        const mouseX = event.clientX;

        const relativeMousePosition = mouseX - left;

        const percantageDistanceWithingBoudningBox = (relativeMousePosition)/width;

        // for now we only allow jumps of 10% which equats to 0.5 star rating jump
        const sizeOfRatingWindow = 1/10;
        const percantageRoundedToAlloweValues =  Math.ceil(percantageDistanceWithingBoudningBox/sizeOfRatingWindow) * sizeOfRatingWindow;

        const reviewStars = document.querySelector("#form-input-stars");

        reviewStars.style.setProperty("--rating", 5.0*percantageRoundedToAlloweValues);
    }

    async function addReview(event)
    {
        event.preventDefault();

        const reviewContent = document.getElementById("review-content");
        const content = reviewContent.value;
        const reviewStars = document.querySelector("#form-input-stars");
        const rating = parseFloat(reviewStars.style.getPropertyValue("--rating")).toFixed(1);
        const date = moment().format();
        const payload = {
            bookId,
            content,
            rating,
            date
        }
        const result = await fetch(`${config.url}/api/v1/reviews`,
        {
            method: "POST",
            credentials: "include",
            headers: {
                "Content-Type": "application/json",
            },
            body:JSON.stringify(payload)
        });

        if(result.ok)
        {
            debugger;
            // instead of refetching all we just append the new review as if it came from server
            // we can do that cause we know it has been accepted
            const temporaryReviewElement = {
                content,
                rating,
                createdAt: date,
                user:{
                    userName: user.userName,
                    id: user.id
                }
            }
            setReviews(currentReviews => [temporaryReviewElement,...currentReviews])
            setUserHasReviewedThisBook(true);
        }
    }


    return (
        <form className="add-review-form" onSubmit={addReview}>
            <p>Add your review</p>
            <div className="stars" style={{"--rating": 5.0}} onMouseMove={handleStarsMove} id="form-input-stars"/>
                <textarea className="review-content-textarea" id="review-content"/>
            <button className="add-review-button">Add review</button>
        </form>
    )
}

export default BookAddReviewForm;
import '../styles/reviews.css'

const BookReview = (props) =>
{
    const {review} = props;
    const {user} = review;

    return (
        <div className="book-review-container">
            <h2>{user.userName}</h2>
            <div className="stars" style={{"--rating": review.rating}}></div>
            <p>{review.content}</p>
        </div>
    );
}

export default BookReview;
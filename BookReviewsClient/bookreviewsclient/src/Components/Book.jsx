const Book = ({props})=>
{
    return (
        <div className="book-container">
            <p className={`book-title`}>{props.title}</p>
            <p>{props.year}</p>
            <img className="book-cover" src={props.img} alt="book cover"></img>
        </div>
    );
}

export default Book;
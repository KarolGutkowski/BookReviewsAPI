const Book = ({props})=>
{
    return (
        <div>
            <p>Title: {props.title}</p>
            <p>Year: {props.year}</p>
            <img src={props.img} alt="book cover"></img>
        </div>
    );
}

export default Book;
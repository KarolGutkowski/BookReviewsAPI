import { useContext} from "react";
import UserLoginStateContext from "./UserLoginStateContext";
import searchIcon from "../images/search-icon.png";
import { Link } from "react-router-dom";
import {config} from "../Constants";
import { useNavigate } from "react-router-dom";

const NavigationBar = ({setSearchResultBooks})=>
{
    const {userName,setLoggedIn} = useContext(UserLoginStateContext);
	const navigate = useNavigate();

    async function searchBooks(event)
    {
        event.preventDefault();

		var form = document.forms.searchBooksForm;
		var formData = new FormData(form);

		var searchPhrase = formData.get("searchInput");
		const result = await fetch(`${config.url}/api/v1/books/search/${searchPhrase}`);

		if(!result.ok){
			console.error("Error fetching books");
			return;
		}
		const data = await result.json();

		setSearchResultBooks([...data]);
		navigate("/search");
    }

    const item = 
      <nav>
          <ul className="nav-list">
            <Link to="/">
              <li className="nav-item">Home</li>
            </Link>
            <Link to="/books">
              <li className="nav-item">Catalog</li>
            </Link>
            {userName?
            <Link to="/profile">
                <li className="nav-item">Profile</li>
              </Link>:
            null}
            <li className="nav-item">
              <form className="search-bar" onSubmit={searchBooks} id="searchBooksForm">
                <button type="submit" className="search-icon-button">
                  <img src={searchIcon} alt="search-icon" className="search-icon"/>
                </button>
              <input type="text" className="nav-search-bar" placeholder="Search By Title or Author" name="searchInput"/>
              </form>
            </li>
            <li className="nav-item">
            {userName?
              <Link to="/" onClick={()=>setLoggedIn(null)}>
                <li className="nav-item">Logout</li>
              </Link>:
              <Link to="/login">
                <button className="login-button">Sign In</button>
              </Link>
            }
            </li>
          </ul>
      </nav>
    
    return (
      <div className="nav-container">
        {item}
      </div>
    )
}
export default NavigationBar;
import { useContext} from "react";
import UserLoginStateContext from "./UserLoginStateContext";
import searchIcon from "../images/search-icon.png";
import { Link } from "react-router-dom";

const NavigationBar = ()=>
{
    const {loggedIn,setLoggedIn} = useContext(UserLoginStateContext);
    console.log("isLoggedIn:", loggedIn);

    const item = 
      <nav>
          <ul className="nav-list">
            <Link to="/">
              <li className="nav-item">Home</li>
            </Link>
            <Link to="/books">
              <li className="nav-item">Catalog</li>
            </Link>
            <li className="nav-item">
              <form className="search-bar">
                <button type="submit" className="search-icon-button">
                  <img src={searchIcon} alt="search-icon" className="search-icon"/>
                </button>
              <input type="text" className="nav-search-bar" placeholder="Search By Title or Author"/>
              </form>
            </li>
            <li className="nav-item">
            {loggedIn?
              <Link to="/" onClick={()=>setLoggedIn(false)}>
                <li className="nav-item">Logout</li>
              </Link>:
              <Link to="/login">
                <button className="login-button">Login</button>
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
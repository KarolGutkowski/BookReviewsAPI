import { useContext } from "react";
import UserLoginStateContext from "./UserLoginStateContext";
import searchIcon from "../images/search-icon.png";

const NavigationBar = ()=>
{
    const {isLoggedIn} = useContext(UserLoginStateContext);
    const item = isLoggedIn? 
    <nav>
      <ul className="nav-list">
        <li>
          <li className="nav-item">Home</li>
          <li className="nav-item">Catalog</li>
        </li>
        <li>
          <input type="text" placeholder="Search By Title or Author"/>
        </li>
        <li className="nav-item">Logout</li>
      </ul>
    </nav> : 
    <nav>
        <ul className="nav-list">
          <li className="nav-item">Home</li>
          <li className="nav-item">Catalog</li>
          <li className="nav-item">
            <form className="search-bar">
              <button type="submit" className="search-icon-button">
                <img src={searchIcon} alt="search-icon" className="search-icon"/>
              </button>
            <input type="text" className="nav-search-bar" placeholder="Search By Title or Author"/>
            </form>
          </li>
          <li className="nav-item">
              <button className="login-button">Login</button>
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
import { useContext } from "react";
import UserLoginStateContext from "./UserLoginStateContext";

const NavigationBar = ()=>
{
    const {isLoggedIn} = useContext(UserLoginStateContext);
    const item = isLoggedIn? 
    <nav>
      <ul className="nav-list">
        <li className="nav-item">Home</li>
        <li className="nav-item">Logout</li>
      </ul>
    </nav> : 
    <nav>
        <ul className="nav-list">
          <li className="nav-item">Home</li>
          <li className="nav-item">Login</li>
        </ul>
    </nav>
    
    return (
      <div className="nav-container">
        {item}
      </div>
    )
}
export default NavigationBar;
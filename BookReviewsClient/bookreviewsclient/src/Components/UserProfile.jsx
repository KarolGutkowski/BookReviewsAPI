import { useContext} from "react";
import UserLoginStateContext from "./UserLoginStateContext";

const UserProfile = (props) =>
{
    const {userName} = useContext(UserLoginStateContext)

    
    return (
        userName?
        <p>{userName}</p>:
        <p>You are not logged in</p>
    );
}
export default UserProfile;
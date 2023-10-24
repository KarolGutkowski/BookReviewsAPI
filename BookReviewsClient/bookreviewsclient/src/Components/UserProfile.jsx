import { useContext} from "react";
import UserLoginStateContext from "./UserLoginStateContext";

const UserProfile = (props) =>
{
    const loginContext = useContext(UserLoginStateContext)
    const {userName}= loginContext;
    return (
        userName?
        <p>{userName}</p>:
        <p>You are not logged in</p>
    );
}
export default UserProfile;
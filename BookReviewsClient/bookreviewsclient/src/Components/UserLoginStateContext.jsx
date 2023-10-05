import { createContext } from "react";

const UserLoginStateContext = createContext(
    {
        isLoggedIn: false,
        setLoggedIn: ()=> {}
    });

export default UserLoginStateContext;
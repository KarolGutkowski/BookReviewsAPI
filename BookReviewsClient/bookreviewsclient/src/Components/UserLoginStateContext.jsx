import { createContext } from "react";

const UserLoginStateContext = createContext(
    {
        userName: null,
        setLoggedIn: ()=> {}
    });

export default UserLoginStateContext;

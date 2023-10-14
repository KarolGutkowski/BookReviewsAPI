import React from "react";
import UserLoginStateContext from "./UserLoginStateContext";
import { useContext } from "react";

export default function LoginForm()
{
    const {_,setLoggedIn}= useContext(UserLoginStateContext);

    async function LoginUser(e)
    {
        e.preventDefault();

        const form = e.target;
        const formData = new FormData(form);

        const userName = formData.get("login");
        const password = formData.get("password");
        await fetch("https://localhost:7235/api/v1/login",{
                                method: "POST",
                                credentials: "include",
                                body:JSON.stringify({userName, password}),
                                headers: {
                                    "Content-Type": "application/json",
                                }
        }).then((response)=>
        {
            e.preventDefault();
            if(response.status === 200)
            {
                setLoggedIn(true);
            }
        })      
    }

    return (
        <form onSubmit={LoginUser} method="post" className="login-form">
            <label htmlFor="login">
                Login:
            </label>
            <input type="text" name="login"/>

            <label htmlFor="password">
                Password:
            </label>
            <input type="password" name="password"/>
            <button>Log In</button>
        </form>
    );
}
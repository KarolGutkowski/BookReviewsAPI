import React from "react";
import UserLoginStateContext from "./UserLoginStateContext";
import { useContext } from "react";
import {Navigate} from "react-router-dom"

export default function LoginForm()
{
    const {userName,setLoggedIn}= useContext(UserLoginStateContext);

    async function LoginUser(e)
    {
        e.preventDefault();
        debugger;
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
                console.log(userName);
                setLoggedIn(userName);
            }
        })      
    }

    return (
        <>
        {userName?
        <Navigate from="/login" to="/"/>:
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
        }
        </>      
    );
}
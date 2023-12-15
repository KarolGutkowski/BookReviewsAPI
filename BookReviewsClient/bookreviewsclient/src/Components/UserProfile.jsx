import { useState, useContext, useEffect} from "react";
import UserLoginStateContext from "./UserLoginStateContext";
import {config} from "../Constants";
import Book from "./Book";

const UserProfile = (props) =>
{
    const {user} = useContext(UserLoginStateContext)
    const [userData, setUserData] = useState(null);

    useEffect(()=>
    {
        if(!user)
            return;

        fetch(`${config.url}/api/v1/users`,
            {
                method: "GET",
                credentials: "include",
            })
        .then(response=>
            {
                if(response.status === 200)
                {
                    return response.json();
                }else
                {
                    return null;
                }
            })
        .then((data)=>
            {
                setUserData(data)
            })
        .catch((err)=>
            {
                console.error("error while loading profile");
            });

    },[user]);  

    
    return (
        <>
        {user?
         userData?
            <div className="user-profile-container">
                <h1>{userData.userName}</h1>
                <div className="user-profile-content">
                    <div className="user-data-section">
                        <div className="user-profile-avatar-containter">
                            <img src={userData.profileImage} alt="user avatar" className="user-profile-avatar"/>
                        </div>
                    </div>
                    <div className="liked-books-container">
                        <h3 className="liked-books-title">Books you like</h3>
                    {userData.likedBooks.map(book =>
                        {
                            return <Book key={book.id} book={book} displayLikeImage={false}/>
                        })}
                    </div>
                    </div>
            </div>:
            <div className="user-profile-loader-container">
                <div className="loader" id="loader-7"></div>
            </div>
        :
        <p>You are not logged in</p>}
        </>
    );
}
export default UserProfile;
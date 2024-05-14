import { Post } from "./Post";

export interface User {
    id : string,
    userName : string,
    posts : Post[]
}
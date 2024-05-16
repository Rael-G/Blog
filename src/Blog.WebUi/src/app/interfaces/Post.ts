import { Comment } from "./Comment";
import { Tag } from "./Tag";
import { User } from "./User";

export interface Post {
    id?: string,
    title: string,
    content: string,
    comments: Comment[]
    tags: Tag[],
    user : User
}
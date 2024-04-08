import { Comment } from "./Comment";
import { Tag } from "./Tag";

export interface Post {
    id?: string,
    title: string,
    content: string,
    comments: Comment[]
    tags: Tag[]
}
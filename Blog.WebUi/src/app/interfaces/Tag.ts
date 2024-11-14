import { Post } from "./Post";

export interface Tag {
    id?: string
    name: string
    posts: Post[]
}
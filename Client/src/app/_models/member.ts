import { Photo } from "./photo";

export interface Member {
    id: number;
    userName: string;
    photoUrl:string
    gender: string;
    age: string;
    knownAs: string;
    created: string;
    lastActive: string;
    introduction: string;
    lookingFor: string;
    interests: string;
    city: string;
    country: string;
    photos: Photo[];

}

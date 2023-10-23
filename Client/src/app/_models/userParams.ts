import { User } from "./user";

export class UserParams{
    gender:string;
    minAge=1;
    maxAge=100;
    pageNumber=1;
    pageSize=5;
    orderBy="lastActive";

    constructor(user:User) {
    this.gender=user.gender==='female' ?'male' : 'female';

    }
}
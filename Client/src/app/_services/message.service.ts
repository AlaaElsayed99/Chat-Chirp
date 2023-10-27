import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { getPaginationHeaders, getPagintedResult } from './paginationHealper';
import { Message } from '../_models/messages';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl=environment.baseUrl;

  constructor(private http:HttpClient ,private toastr : ToastrService) {

   }

   getMessages(pageNumber:number,pageSize:number,container:string){
    let params= getPaginationHeaders(pageNumber,pageSize);
    params=params.append('Container',container);
    return getPagintedResult<Message[]> (this.baseUrl+'message',params,this.http)
}
getMessageThread(useName:string){
  return this.http.get<Message[]>(this.baseUrl+'message/thread/'+useName);
}

sendMessage(userName:string,content:string){
  return this.http.post<Message>(this.baseUrl+'message',{recipientUsername: userName,content});
}
deleteMessage(id: number){

  return this.http.delete(this.baseUrl+'message/' +id);


}
}
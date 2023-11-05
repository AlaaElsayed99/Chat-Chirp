import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { getPaginationHeaders, getPagintedResult } from './paginationHealper';
import { Message } from '../_models/messages';
import { ToastrService } from 'ngx-toastr';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { User } from '../_models/user';
import { BehaviorSubject, take } from 'rxjs';
import { Group } from '../_models/group';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl=environment.baseUrl;
  hubUrl=environment.hubUrl; 
  private hubConnection?: HubConnection;
  private messageThreadSource= new BehaviorSubject<Message[]>([]);
  messageThread$=this.messageThreadSource.asObservable();

  constructor(private http:HttpClient ,private toastr : ToastrService) {

   }

   createHubConnection(user:User,otherUsername: string){
    this.hubConnection= new HubConnectionBuilder()
      .withUrl(this.hubUrl+'message?user='+otherUsername, {
        accessTokenFactory:()=> user.token
      }).withAutomaticReconnect()
      .build(); 

      this.hubConnection.start().catch(err=> console.log(err));
      this.hubConnection.on('ReceiveMessageThread',messages=>{
        this.messageThreadSource.next(messages);
      })
      
      this.hubConnection.on('UpdateGroup',(group:Group)=>{
        if(group.connections.some(x=>x.username===otherUsername))
        {
          this.messageThread$.pipe(take(1)).subscribe({
            next:messages=>{
              messages.forEach(message=> {
                if(!message.dateRead){
                  message.dateRead= new Date(Date.now())
                }
              })

              this.messageThreadSource.next([...messages]);
            }
          })
        }
      })

      this.hubConnection.on('NewMessage', message=> {
        this.messageThread$.pipe(take(1)).subscribe({
          next:messages=>{
            this.messageThreadSource.next([...messages,message])
          }
        })
      })

   }
   stropHubConnection(){
    if(this.hubConnection){
     this.hubConnection?.stop();
    }
   }

   getMessages(pageNumber:number,pageSize:number,container:string){
    let params= getPaginationHeaders(pageNumber,pageSize);
    params=params.append('Container',container);
    return getPagintedResult<Message[]> (this.baseUrl+'message',params,this.http)
}
getMessageThread(useName:string){
  return this.http.get<Message[]>(this.baseUrl+'message/thread/'+useName);
}

async sendMessage(userName:string,content:string){
  // return this.http.post<Message>(this.baseUrl+'message',{recipientUsername: userName,content});
  return this.hubConnection?.invoke('SendMessage',{recipientUsername: userName, content})
  .catch(err=>console.log(err));
}
deleteMessage(id: number){

  return this.http.delete(this.baseUrl+'message/' +id);


}
}
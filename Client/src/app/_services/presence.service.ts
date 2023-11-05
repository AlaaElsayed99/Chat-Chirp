import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment.development';
import { User } from '../_models/user';
import { BehaviorSubject, take } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {


  hubUrl= environment.hubUrl;
  private hubConnection?: HubConnection;
  private onlineUsresSource= new BehaviorSubject<string[]>([]);
  onlineUsers$= this.onlineUsresSource.asObservable();
  constructor(private toastr: ToastrService, private router: Router) { }
  createHubConnection(user:User){
    this.hubConnection= new HubConnectionBuilder()
    .withUrl(this.hubUrl+'presence', {
      accessTokenFactory:() => user.token
    })
    .withAutomaticReconnect()
    .build();

    this.hubConnection.start().catch(err=> console.log(err));
    this.hubConnection.on('UserIsOnline',username=>{
      this.onlineUsers$.pipe(take(1)).subscribe({
        next:usernames=> this.onlineUsresSource.next([...usernames, username])
      })
    })

    this.hubConnection.on('UserIsOffline',username=>{
      this.onlineUsers$.pipe(take(1)).subscribe({
        next:usernames=> this.onlineUsresSource.next(usernames.filter(x=>x!== username))
      })
    })

    this.hubConnection.on('GetOnlineUsers',usernames=>{
      this.onlineUsresSource.next(usernames);
    })
    this.hubConnection.on('NewMessageReceived',({username,knownAs})=>{
      this.toastr.info(knownAs+ ' has send a new message! Click to see it')
      .onTap.pipe(take(1)).subscribe({
        next:()=> this.router.navigateByUrl('/members/' +username+ '?tab=Messages')
      })
    })
  }
  stopHubConnection(){
    this.hubConnection?.stop().catch(err=>console.log(err));
  }


}

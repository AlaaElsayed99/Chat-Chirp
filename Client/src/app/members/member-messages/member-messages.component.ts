import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { Message } from 'src/app/_models/messages';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-messages',
  standalone:true,
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css'],
  imports:[CommonModule,TimeagoModule,FormsModule]
})

export class MemberMessagesComponent implements OnInit {

  @ViewChild('messageForm') messageForm?:NgForm
  @Input() userName?:string ;
  messageContent = '';
  
  constructor(public messageService:MessageService) {
    
  }
  ngOnInit(): void {
  }

  sendMessage(){
    
    if(!this.userName) return;
    this.messageService.sendMessage(this.userName,this.messageContent).then(()=>{
      this.messageForm?.reset();
    })
    // .subscribe({
    //   next: message=> {
    //     // this.messages.push(message);
    //     // this.messageForm?.reset();
        

      
    //   },
    //   error:(err)=>{console.log("Error sending message", err)}
    // })
  }
}

  


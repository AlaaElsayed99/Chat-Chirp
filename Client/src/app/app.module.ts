import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { ToastrModule, provideToastr } from 'ngx-toastr';
import { SharedModule } from './_modeules/shared.module';
import { TestsErrorComponent } from './error/tests-error/tests-error.component';
import { ErrorInterceptor } from './_interceptor/error.interceptor';
import { NotFoundComponent } from './error/not-found/not-found.component';
import { ServerErrorComponent } from './error/server-error/server-error.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { JwtInterceptor } from './_interceptor/jwt.interceptor';
import { TabsModule } from 'ngx-bootstrap/tabs';


@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    ListsComponent,
    MessagesComponent,
    TestsErrorComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberCardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    SharedModule,
    TabsModule.forRoot()
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS,useClass:ErrorInterceptor,multi:true},
    {provide: HTTP_INTERCEPTORS,useClass:JwtInterceptor,multi:true},

    provideToastr({
      timeOut: 6000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    }), 
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

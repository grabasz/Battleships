import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { BoardComponent } from './board/board.component';
import { WaitingComponent } from './waiting/waiting.component';
import {MatButtonToggleModule} from '@angular/material/button-toggle';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material';
import { HttpClientModule } from '@angular/common/http';
import { InsertionOptionsComponent } from './insertion-options/insertion-options.component';

@NgModule({
  declarations: [
    AppComponent,
    BoardComponent,
    WaitingComponent,
    InsertionOptionsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    MatButtonToggleModule,
    MatButtonModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

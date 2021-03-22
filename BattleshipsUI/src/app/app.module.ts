import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './components/app.component';
import { BoardComponent } from './components/board/board.component';
import { WaitingComponent } from './components/waiting/waiting.component';
import {MatButtonToggleModule} from '@angular/material/button-toggle';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material';
import { HttpClientModule } from '@angular/common/http';
import { InsertionOptionsComponent } from './components/insertion-options/insertion-options.component';
import { GameResultComponent } from './components/game-result/game-result.component';

@NgModule({
  declarations: [
    AppComponent,
    BoardComponent,
    WaitingComponent,
    InsertionOptionsComponent,
    GameResultComponent
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

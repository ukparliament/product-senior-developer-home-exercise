import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { PersonManagementComponent } from './components/person/personManagement/personManagement.component';
import { PersonListComponent } from './components/person/personlist/person-list.component';
import { PersonEditorComponent } from './components/person/personeditor/person-editor.component';
import { HeaderComponent } from './components/shared/header/header.component';
import { FooterComponent } from './components/shared/footer/footer.component';

@NgModule({
  declarations: [
    AppComponent,
    PersonManagementComponent,
    PersonListComponent,
    PersonEditorComponent,
    HeaderComponent,
    FooterComponent
  ],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: PersonManagementComponent, pathMatch: 'full' }])],
  providers: [provideHttpClient(withInterceptorsFromDi())]
})

export class AppModule { }

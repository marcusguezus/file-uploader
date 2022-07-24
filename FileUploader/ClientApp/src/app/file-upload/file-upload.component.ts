import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'file-upload',
    templateUrl: './file-upload.component.html',
    styleUrls: ['./file-upload.component.css']
})

export class FileUploadComponent implements OnInit {
    public message: string;
    public errorMessage: string;
    public progress: number;
    @Output() public onUploadFInished = new EventEmitter();

    // Inject service 
    constructor(private http: HttpClient, private router: Router) { }

    ngOnInit(): void {
        console.log(this.router.url);
    }

    public uploadFile = (files) => {
        if (files.length === 0) return;

        let fileToUpload = <File>files[0];
        const formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);

        this.http.post(location.origin + '/api/fileupload', formData, { reportProgress: true, observe: 'events' })
            .subscribe(
                (event) => {
                    debugger;
                    if (event.type === HttpEventType.UploadProgress) {
                        this.progress = Math.round(100 * event.loaded / event.total);
                    }
                    else if (event.type === HttpEventType.Response) {
                        this.message = event.statusText;
                        this.onUploadFInished.emit(event.body);
                    }
                },
                (error) => {
                    debugger;
                    this.errorMessage = error.error;
                }
            );
    }
}
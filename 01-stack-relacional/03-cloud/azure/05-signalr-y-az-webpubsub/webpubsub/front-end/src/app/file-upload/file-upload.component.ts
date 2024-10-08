import { Component, Input, OnInit, EventEmitter, Output } from '@angular/core';
import { BlobServiceClient } from '@azure/storage-blob';
import { environment } from 'src/environments/environment';
import { Hero } from '../hero';
import { HeroService } from '../hero.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {

  imgName: string = '';
  imgFile?: string;
  fileType: string = '';
  image: any;

  @Input() hero!: Hero;
  @Output() messageEvent = new EventEmitter<string>();


  constructor(private heroService: HeroService) { }

  ngOnInit(): void {
  }

  onImageChange(e: any): void {
    const reader = new FileReader();

    if (e.target.files && e.target.files.length) {
      const [file] = e.target.files;
      reader.readAsArrayBuffer(file);

      reader.onload = () => {

        if (file.type != "image/jpeg" && file.type != "image/png") {
          alert('Invalid file');
        }
        else {
          this.imgName = file.name;
          this.image = reader.result;
          this.fileType = file.type;

          console.log(file.type);
        }
      }
    }
  }

  async upload() {

    const finalImageName = `${this.hero.alterEgo.replace(' ', '-').toLowerCase()}${this.imgName.slice(this.imgName.lastIndexOf('.'))}`;

    console.log(`final image name: ${finalImageName}`);

    this.heroService.getSasToken(finalImageName).subscribe(async (uriSas) => {
      console.log(`uriSas: ${uriSas}`);

      const blobClient = new BlobServiceClient(uriSas);
      const containerClient = blobClient.getContainerClient(environment.containerName);

      console.log(`Final image name: ${finalImageName}`);

      const blobFile = containerClient.getBlockBlobClient(finalImageName);

      await blobFile.uploadData(this.image, {
        concurrency: 20,
        onProgress: (ev) => console.log(ev),
        blobHTTPHeaders: { blobContentType: this.fileType }
      });

      this.messageEvent.emit('newAlterEgoImage');

    });
  }
}

import axios from "axios";
import { useState } from "react";

interface IUploaderProps {
  id: string
}
export const Uploader = (props: IUploaderProps) => {
  const [file, setFile] = useState<any>()

  function handleChange(event: any) {
    setFile(event.target.files[0])
  }

  function handleSubmit(event: any) {
    event.preventDefault()
    const url = process.env.REACT_APP_API_URL + "Games/" + props.id + "/Screenshots/Upload";
    const formData = new FormData();
    formData.append('formFile', file!);
    formData.append('fileName', file!.name);
    const config = {
      headers: {
        'content-type': 'multipart/form-data',
      },
    };
    axios.post(url, formData, config).then((response) => {
      console.log(response.data);
    });

  }

  return (
    <div className="App">
      <form onSubmit={handleSubmit}>
        <h1>Upload Screenshots</h1>
        <input type="file" onChange={handleChange} />
        <button type="submit">Upload</button>
      </form>
    </div>
  );
}
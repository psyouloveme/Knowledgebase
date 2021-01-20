def error_and_exit(error_message)
  print error_message
  exit
end

argv_size = ARGV.size
((argv_size > 1) and
error_and_exit("Expected 1 argument (found #{argv_size}).")) or
((argv_size == 0) and
error_and_exit("Expected 1 argument, found 0."))

filename = ARGV[0]
bytes = IO.binread(filename)
word_size = 4
lines_last_index = bytes.size/word_size - 1 # fist index is 0

new_line_str    = "\n"
pack_command    = "c*"
unpack_command  = "e"

output_file = File.open("coli_floats.txt", "w+")

index = 0
bytes.each_byte.each_slice(word_size) { |word|
  index += 1

  first_3_bytes_sum = word[3] + word[2] + word[1]
  word_sum          = first_3_bytes_sum + word[0]
  
  ((first_3_bytes_sum == 0) or
  (word_sum == 1020)) and
  next
  # Exclude words from [00 00 00 01] to [00 00 00 ff],
  # and words equal to [ff ff ff ff].

  output_file.print word.pack(pack_command).unpack(unpack_command)[0]

  (index < lines_last_index) and output_file.print new_line_str
}

output_file.close